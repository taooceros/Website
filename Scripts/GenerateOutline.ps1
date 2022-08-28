cd wwwroot
$items = Get-ChildItem Blogs -Recurse -Include *.md.encrypted

$res = foreach($item in $items){
    @{
        FileName = Resolve-Path $item -Relative | Split-Path -leaf
        FilePath = Resolve-Path $item -Relative
    }
}

$res | ConvertTo-Json -Compress -AsArray | Out-File -FilePath Blogs\outline.json -Encoding utf8

cd ..