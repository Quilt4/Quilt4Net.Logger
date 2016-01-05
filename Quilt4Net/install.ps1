param($installPath, $toolsPath, $package, $project)

try
{
  $url = "http://ci.quilt4.com/master/web/#/install?version=" + $package.Version
  $dte2 = Get-Interface $dte ([EnvDTE80.DTE2])
  $dte2.ItemOperations.Navigate($url) | Out-Null
}
catch
{
}