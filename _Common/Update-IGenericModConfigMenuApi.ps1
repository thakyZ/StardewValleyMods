using namespace System;
using namespace System.IO;
using namespace System.Text.RegularExpressions;
using namespace Microsoft.PowerShell.Commands;

[CmdletBinding()]
Param()

Begin {
  [string] $FileUri = 'https://raw.githubusercontent.com/spacechase0/StardewValleyMods/develop/GenericModConfigMenu/IGenericModConfigMenuApi.cs';
  [FileInfo] $DestinationFile = (Get-Item -LiteralPath (Join-Path -Path $PSScriptRoot -ChildPath 'Integrations' -AdditionalChildPath @('IGenericModConfigMenuApi.cs')) -ErrorAction Stop);
} Process {
  [BasicHtmlWebResponseObject] $FileDownload = (Invoke-WebRequest -Uri $FileUri -SkipHttpErrorCheck -UserAgent ([Microsoft.PowerShell.Commands.PSUserAgent]::Chrome));
  If ($Null -eq $FileDownload) {
    Write-Error -Message 'Unknown error when trying to download file.';
    Exit 1;
  } Else {
    If ($FileDownload.StatusCode -ne 200) {
      Write-Error -Message "Download of file from GitHub filed with status code $($FileDownload.StatusCode).";
      Exit 1;
    } Else {
      [string] $Content = $FileDownload.Content;
      [Regex] $Regex1 = [Regex]::new('(?<!\?)( [a-zA-Z_]+ ?= ?null)', [RegexOptions]::Multiline);
      $Content = $Regex1.Replace($Content, '?$1');
      [Regex] $Regex2 = [Regex]::new('\r?\n\s+: .*? // DELETE THIS LINE WHEN COPIED INTO YOUR MOD CODE', [RegexOptions]::Multiline);
      $Content = $Regex2.Replace($Content, '');
      [Regex] $Regex3 = [Regex]::new('\r?\nusing GenericModConfigMenu\.Framework;', [RegexOptions]::Multiline);
      $Content = $Regex3.Replace($Content, '');
      Set-Content -Value $Content -LiteralPath $DestinationFile -Encoding UTF8;
    }
  }
} End {
  Exit 0;
}
