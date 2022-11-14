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

def main [key_base64: string] {
    let files = (ls wwwroot\Blogs\**\*.md | get name)

    let key_str = ($key_base64 | base64_tohex)
        
    let iv = (0..31 | each {"0"} | str join)

    $files | par-each { |file|
       let content = (open $file)
       let enc_content = ($content 
                            | encode base64 
                            | openssl enc -e -aes-256-cbc -K $key_str -iv $iv -a -out $"($file).encrypted")
    }

    let encrypted_files = (ls wwwroot\Blogs\**\*.md.encrypted | get name)

    generate_outline $encrypted_files
}

