# def base64_tohex [] {
#     let $base64_string = $in
#     let $binary_rep = ($base64_string | decode base64 --binary)

#     (0..(($binary_rep | bytes length) - 1)) | 
#         each { |$i|
#             let $byte = ($binary_rep | bytes at $i..($i + 1))
#             let $hex = ($byte | format "%02x")
#             $hex
#         }
# }

def generate_outline [files : list] {
    let $files = $files
    let $outline = ($files | each {|it| 
            let $file = ($it | path relative-to "wwwroot")
            let $filename = ($file | path basename)
            {FileName : $filename, FilePath: $file}
        })
    $outline | to json | save "wwwroot/Diaries/outline.json" --raw -f
}

def get_modified [] {
    let files = $in
    let cache_path = ".cache.json"
    let cache = if ($cache_path | path exists) {open $cache_path} else {$files | each {|it| {name:$it val:""}} | transpose -rid}
    let newcache = ($files | par-each {|it| {path: $it, hash:(open $it | hash md5)}} | transpose -rid)
    $newcache | save $cache_path -f


    let $modified = ($files | each { |it| 
        let $path = $it
        let $hash = ($path | open | hash md5)
        if ((not $path in $cache) or ($hash != ($cache | get $path))) { $path } else { null }
        })
    $modified
}

def main [key: string] {
    let files = (ls wwwroot\Diaries\**\*.md | get name)

    let key_str = ($key)
        
    let iv = (0..31 | each {"0"} | str join)

    let modified = ($files | get_modified)

    echo $modified

    $modified | par-each { |file|
       let content = (open $file)
       let enc_content = ($content 
                            | encode base64 
                            | ^'C:\Program Files\Git\mingw64\bin\openssl.exe' enc -e -aes-256-cbc -K $key_str -iv $iv -a -out $"($file).encrypted")
    }

    let encrypted_files = (ls wwwroot\Diaries\**\*.md.encrypted | get name)

    generate_outline $encrypted_files

    git add "*.md.encrypted"
    git add "*/outline.json"
    git commit -m $"Diary Update at (date now | format date %Y-%m-%d)"
    git push
}

