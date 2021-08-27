# Vault-VCET-Build-Configurator
MS Build Task which configures Vault Extension VCET files to the referenced DLLs automatically.
 <UsingTask TaskName="SoupSoftware.VCETConfigurator" AssemblyFile="c:\temp\VCETConfigurator.dll">


 <Target Name="AfterBuild" AfterTargets="AfterBuild">
    <VCETConfigurator InputFilename="$(OutputPath)" referencePath="$(AutoDeskSDKSource)" />
  </Target>

