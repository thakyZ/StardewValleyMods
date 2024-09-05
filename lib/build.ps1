Begin {
  Push-Location -LiteralPath $PSScriptRoot
} Process {
  pyinstaller --onefile (Join-Path -Path $PSScriptRoot -ChildPath "patch_json.py");

  Copy-Item -LiteralPath (Join-Path -Path $PSScriptRoot -ChildPath "dist" -AdditionalChildPath @("patch_json.exe")) -Destination  (Join-Path -Path $PSScriptRoot -ChildPath "patch_json.exe");
} Clean {
  Pop-Location
}
