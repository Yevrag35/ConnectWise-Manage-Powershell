param
(
    [parameter(Mandatory = $true, Position = 0)]
    [string] $DebugDirectory,

    [parameter(Mandatory = $true, Position = 1)]
    [string] $ModuleFileDirectory,

	[parameter(Mandatory = $true, Position = 2)]
	[string] $AssemblyInfo,

    [parameter(Mandatory = $true, Position = 3)]
    [string] $TargetFileName
)

$curDir = Split-Path -Parent $MyInvocation.MyCommand.Definition;

## Clear out files
Get-ChildItem -Path $DebugDirectory -Include *.ps1xml -Recurse | Remove-Item -Force;

## Get Module Version from project.assets.json - PowerShell Core
#$json = Get-Content "$curDir\obj\project.assets.json" | ConvertFrom-Json;
#$vers = $json.project.version;

## Get Module Version from Assembly.cs - WindowsPowerShell
$assInfo = Get-Content -Path $AssemblyInfo;
foreach ($line in $assInfo)
{
    if ($line -like "*AssemblyFileVersion(*")
    {
        $vers = $line -replace '^\s*\[assembly\:\sAssemblyFileVersion\(\"(.*?)\"\)\]$', '$1';
    }
}

$allFiles = Get-ChildItem $ModuleFileDirectory -Include * -Exclude *.old -Recurse;
#$References = Join-Path "$ModuleFileDirectory\.." "Assemblies";

[string[]]$verbs = Get-Verb | Select-Object -ExpandProperty Verb;
$patFormat = '^({0})(\S{{1,}})\.cs';
$pattern = $patFormat -f ($verbs -join '|');
$cmdletFormat = "{0}-{1}";

$baseCmdletDir = Join-Path "$ModuleFileDirectory\.." "Cmdlets";
[string[]]$folders = [System.IO.Directory]::EnumerateDirectories($baseCmdletDir, "*", [System.IO.SearchOption]::TopDirectoryOnly) | Where-Object { -not $_.EndsWith('Bases') };

$aliasPat = '\[(?:a|A)lias\(\"(.{1,})\"\)\]'
$csFiles = @(Get-ChildItem -Path $folders *.cs -File);
$Cmdlets = New-Object System.Collections.Generic.List[string] $csFiles.Count;
$Aliases = New-Object System.Collections.Generic.List[string];
foreach ($cs in $csFiles)
{
	$match = [regex]::Match($cs.Name, $pattern)
    $Cmdlets.Add(($cmdletFormat -f $match.Groups[1].Value, $match.Groups[2].Value));
    $content = Get-Content -Path $cs -Raw;
    $aliasMatch = [regex]::Match($content, $aliasPat, [System.Text.RegularExpressions.RegexOptions]::IgnoreCase);
    if ($aliasMatch.Success)
    {
        $Aliases.Add($aliasMatch.Groups[1].Value);
    }
}

#[string[]]$allDlls = Get-ChildItem $References -Include *.dll -Exclude 'System.Management.Automation.dll' -Recurse | Select-Object -ExpandProperty Name;
[string[]]$allFormats = $allFiles | Where-Object -FilterScript { $_.Extension -eq ".ps1xml" } | Select-Object -ExpandProperty Name;

$manifestFile = "CWManage.psd1"

$allFiles | Copy-Item -Destination $DebugDirectory -Force;
$modPath = Join-Path $DebugDirectory $manifestFile;

$manifest = @{
    Path                   = $modPath
    Guid                   = '479ca402-7c7c-4d29-aa58-eb809064cd77';
    Description            = 'A PowerShell wrapper for the ConnectWise Manage REST API.'
    Author                 = 'Mike Garvey'
    CompanyName            = 'Yevrag35, LLC.'
    Copyright              = '(c) 2019 Yevrag35, LLC.  All rights reserved.'
    ModuleVersion          = $($vers.Trim() -split '\.' | Select-Object -First 3) -join '.'
    PowerShellVersion      = '5.1'
    DotNetFrameworkVersion = '4.5.2'
    RootModule             = $TargetFileName
    DefaultCommandPrefix   = "CWM"
#    RequiredAssemblies     = $allDlls
	CmdletsToExport		   = $Cmdlets
#	CompatiblePSEditions   = "Core", "Desktop"
    FormatsToProcess       = if ($allFormats.Length -gt 0) { $allFormats } else { @() };
    ProjectUri             = 'https://github.com/Yevrag35/ConnectWise-Manage-Powershell'
#	LicenseUri			   = 'https://raw.githubusercontent.com/Yevrag35/PoshSonarr/master/LICENSE'
	HelpInfoUri			   = 'https://github.com/Yevrag35/ConnectWise-Manage-Powershell/issues'
#    Tags                   = @()
};

New-ModuleManifest @manifest;
#Update-ModuleManifest -Path $modPath -Prerelease 'alpha' -FunctionsToExport '';