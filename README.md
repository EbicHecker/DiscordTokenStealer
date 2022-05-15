<h1>Discord Token Stealer</h1>

<figure>
    <figcaption><h3>Grabs</h3></figcaption>
    <ul>
      <li>PC Username</li>
      <li>Machine Name</li>
      <li>Operating System</li>
      <li>IP Address</li>
      <li>Discord accounts and their tokens parsed from targets below.</li>
    </ul>
    <figcaption><h3>Targets</h3></figcaption>
    <ul>
      <li>Discord</li>
      <li>DiscordPTB</li>
      <li>Discord Canary</li>
      <li>Chrome</li>
      <li>Firefox</li>
      <li>Opera</li>
      <li>Brave</li>
      <li>Yandex</li>
      <li>Easy to add more locations to parse from.</li>
    </ul>
</figure>

<h2>TODO:</h2>
<ul>
    <li>Use something like <a href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing">Microsoft.Extensions.FileSystemGlobbing</a> for finding potentially vulnerable files, this would avoid hardcoded paths and would increase the possibilities.</li>
    <li>Obfuscation (ex: String Encryption, Compressor, Renaming, etc.) to avoid detection.</li>
    <li>
        Support more platforms. 
        Downgrade .NET Version or move to a native/more supported programming language. 
        Current version requires the .NET 6.0 runtime to be installed.
    </li>
</ul>

<h2>Preview:</h2>
<p align="left">
  <img src="https://i.imgur.com/QA20fHd.png" height=250 width=600 title="Windows">
</p>
