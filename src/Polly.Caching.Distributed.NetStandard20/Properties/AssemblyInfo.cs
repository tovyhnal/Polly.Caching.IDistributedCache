using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Polly.Caching.Distributed")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.5.0")]
[assembly: AssemblyInformationalVersion("1.0.5.0")]
[assembly: CLSCompliant(false)] // Because Microsoft.Extensions.Caching.Memory.IDistributedCache, on which Polly.Caching.IDistributedCache.NetStandard11 depends, is not CLSCompliant.

[assembly: InternalsVisibleTo("Polly.Caching.Distributed.NetStandard20.Specs")]