cd wwwroot\Diaries

let year = (date now | date format "%Y")
let month = (date now | date format "%m")
let day = (date now | date format "%d")


let dir = $"($year)-($month)"
let file_name = $"($day).md"

mkdir $dir
cd $dir

if not ($file_name | path exists) {
	echo $"($month)/($day)\n" | save $file_name
}


^'C:\Program Files\Sublime Text\subl.exe' $file_name