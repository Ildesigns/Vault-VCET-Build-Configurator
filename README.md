# Vault-VCET-Build-Configurator
MS Build Task which configures Vault Extension VCET files to the referenced DLLs automatically.
Instructions for use:1. Build this project file.2. Modify the Project file for the vault extension build, adding a configuration for each version of vault dlls to build for.3. Modify (in notepad) the csproj or vbproj for the vault extension making the following changes..    a. Add the following line in the <Project> node, update the path of the AssemblyFile to the location of the dll.                <UsingTask TaskName="SoupSoftware.VCETConfigurator" AssemblyFile="c:\temp\VCETConfigurator.dll">
    b. add the following parameter into each configuration updating the path to the correct dll location to build from
                 <AutoDeskSDKSource>C:\Program Files\Autodesk\Vault Professional 2018\Explorer</AutoDeskSDKSource>
    c. Delete the <TargetFrameworkVersion> node from the master project group.
    d. Add a  <TargetFrameworkVersion> node to each build configuration.
    e. Add the following node to the project file bewfore the </Project> node.
            <Target Name="AfterBuild" AfterTargets="AfterBuild">
            <VCETConfigurator InputFilename="$(OutputPath)" referencePath="$(AutoDeskSDKSource)" />
            </Target>



