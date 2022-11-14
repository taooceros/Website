def base64_tohex [] {
    let $base64_string = $in
    let binary = ($base64_string | decode base64 --binary)
    ($"($binary)" 
        | str substring [1,-1] 
        | split row "," 
        | each {|it| 
            ($it 
            | into int 
            | fmt 
            | get upperhex 
            | into string 
            | str substring "2," 
            | str lpad -l 2 -c '0')
          }
        | str join)
}

def generate_outline [files : list] {
    let $files = $files
    let $outline = ($files | each {|it| 
            let $file = ($it | path relative-to "wwwroot")
            let $filename = ($file | path basename)
            {FileName : $filename, FilePath: $file}
        })
    $outline | to json | save "wwwroot/Blogs/outline.json" --raw
}

def get_modified [] {
    let files = $in
    let cache_path = ".cache.json"
    let cache = if ($cache_path | path exists) {open $cache_path} else {$files | each {|it| {name:$it val:""}} | transpose -rid}
    let newcache = ($files | par-each {|it| {path: $it, hash:(open $it | hash md5)}} | transpose -rid)
    $newcache | save $cache_path


    let $modified = ($files | each { |it| 
        let $path = $it
        let $hash = ($path | open | hash md5)
        let $oldhash = ($cache | get $path)
        if ($oldhash != $hash) { $path } else { null }
        })
    $modified
}

def main [key_base64: string] {
    let files = (ls wwwroot\Blogs\**\*.md | get name)

    let key_str = ($key_base64 | base64_tohex)
        
    let iv = (0..31 | each {"0"} | str join)

    let modified = ($files | get_modified)

    echo $modified

    $modified | par-each { |file|
       let content = (open $file)
       let enc_content = ($content 
                            | encode base64 
                            | openssl enc -e -aes-256-cbc -K $key_str -iv $iv -a -out $"($file).encrypted")
    }

    let encrypted_files = (ls wwwroot\Blogs\**\*.md.encrypted | get name)

    generate_outline $encrypted_files

    git add "*.md.encrypted"
    git add "*/outline.json"
    git commit -m $"Blog Update (date format %Y-%m-%d)"
}

