# Bower Registry
C# implmentation of the bower registry.

Default implementation has __0__ dependencies on external DBs. Default implementation can be overwritten by implementing an IPackageRepository.

## Install
```
git clone git@github.com:dipeshc/Bower-Registry.git
cd bower-registry
msbuild Bower-Registry.sln
```


## How to use
### The console
```
BowerRegistry server -p <port number> 
```

## Examples
```
BowerRegistry server -p 8080
```

## Command Line Options
```
'server' - hosts the bower registry.

Expected usage: BowerRegistry.exe server <options> 
<options> available:
  -p, --port=VALUE           Specifies the port that the server will listen 
                               on [default 80].
  -j, --json=VALUE           Path to json document containing serialized 
                               package repository [defaults to "packages.json" 
                               if neither --json or --xml provided].
  -x, --xml=VALUE            Path to xml document containing serialized 
                               package repository.
```

## License
MIT
