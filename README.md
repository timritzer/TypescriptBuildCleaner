# TypescriptBuildCleaner
Allows cleaning of Typescript build outputs


Cleans all .js.map files in the targetted directory, and the associated .js file if the map exists.

Allows for easy use of the clean to clean up js files and prevent errors with moving files or renaming.

Add the following to your project to enable Visual Studio to run this as a part of the normal clean:

    <Target Name="AfterClean">
        <Exec Command='TypescriptCleaner.exe "$(ProjectDir) " '/>
    </Target>
