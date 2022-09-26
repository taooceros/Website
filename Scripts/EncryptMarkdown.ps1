function Create-AesManagedObject($key, $IV)
{
    $aesManaged = [System.Security.Cryptography.Aes]::Create();
    $aesManaged.Mode = [System.Security.Cryptography.CipherMode]::CBC
    $aesManaged.BlockSize = 128
    $aesManaged.KeySize = 256
    if ($IV)
    {
        if ($IV.getType().Name -eq "String")
        {
            $aesManaged.IV = [System.Convert]::FromBase64String($IV)
        }
        else
        {
            $aesManaged.IV = $IV
        }
    }
    
    $aesManaged.IV = [System.Byte[]]::CreateInstance([System.Byte],16)
    
    if ($key)
    {
        if ($key.getType().Name -eq "String")
        {
            $aesManaged.Key = [System.Convert]::FromBase64String($key)
        }
        else
        {
            $aesManaged.Key = $key
        }
    }
    $aesManaged
}

function Create-AesKey()
{
    $aesManaged = Create-AesManagedObject
    $aesManaged.GenerateKey()
    [System.Convert]::ToBase64String($aesManaged.Key)
}

function Encrypt-String($key, $unencryptedString)
{
    $bytes = [System.Text.Encoding]::UTF8.GetBytes($unencryptedString)
    $aesManaged = Create-AesManagedObject $key 
    $encryptor = $aesManaged.CreateEncryptor()
    $encryptedData = $encryptor.TransformFinalBlock($bytes, 0, $bytes.Length);
    [byte[]]$fullData = $aesManaged.IV + $encryptedData
    $aesManaged.Dispose()
    [System.Convert]::ToBase64String($fullData)
}

function Decrypt-String($key, $encryptedStringWithIV)
{
    $bytes = [System.Convert]::FromBase64String($encryptedStringWithIV)
    $IV = $bytes[0..15]
    $aesManaged = Create-AesManagedObject $key 
    $decryptor = $aesManaged.CreateDecryptor();
    $unencryptedData = $decryptor.TransformFinalBlock($bytes, 16, $bytes.Length - 16);
    $aesManaged.Dispose()
    [System.Text.Encoding]::UTF8.GetString($unencryptedData).Trim([char]0)
}

$key = "If you find this, and are interested in reading my blog, contact me on hongtao_zhang@outlook.com with Welcome!"
$key = $args[0]

$files = Get-ChildItem "wwwroot" -Recurse -Include "*.md"

foreach ($file in $files)
{
    $content = Get-Content $file -Encoding UTF8 -Raw
    $encryptedContent = Encrypt-String $key $content
    $encryptedContent | Out-File -Path "$file.encrypted" -Encoding UTF8
    #    Decrypt-String $key $encryptedContent | Write-Output
    #    Remove-Item $file
}
