$cert_store = new-object System.Security.Cryptography.X509Certificates.X509Store("My", "LocalMachine")
$cert_store.Open("ReadOnly")
$tokensigning_cert = ($cert_store.Certificates | where-object { $_.Subject -eq "CN=LocalEscStsTokenSigning" })

#backup copy the current web.config file
$executingScriptDirectory = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent
$webConfigPath = $executingScriptDirectory + "\web.config"   
$currentDate = (get-date).tostring("yyyy_MM_dd-hh_mm_s") # month_day_year - hours_mins_seconds   
$backup = $webConfigPath + "_$currentDate"   

# Get the content of the config file and cast it to XML and save a backup copy labeled .bak followed by the date  
$xml = [xml](get-content $webConfigPath)  

#save a backup copy  
$xml.Save($backup)  

#get document
$root = $xml.get_DocumentElement();  

#get the element
$node = $root."system.identitymodel".identityconfiguration.issuerNameRegistry.trustedIssuers.add

#change the thumbprint
$node.thumbprint = $tokensigning_cert.Thumbprint

# Save it  
$xml.Save($webConfigPath)
# SIG # Begin signature block
# MIIJKAYJKoZIhvcNAQcCoIIJGTCCCRUCAQExCzAJBgUrDgMCGgUAMGkGCisGAQQB
# gjcCAQSgWzBZMDQGCisGAQQBgjcCAR4wJgIDAQAABBAfzDtgWUsITrck0sYpfvNR
# AgEAAgEAAgEAAgEAAgEAMCEwCQYFKw4DAhoFAAQUMSm/g7NMIJ3FayBuIwH48Ic7
# VnCgggagMIIGnDCCBYSgAwIBAgIKYYLohAAAAABu4zANBgkqhkiG9w0BAQUFADBD
# MRUwEwYKCZImiZPyLGQBGRYFcmFkaXgxFjAUBgoJkiaJk/IsZAEZFgZuYXRpb24x
# EjAQBgNVBAMTCUVEQ3Byb2RDQTAeFw0xMjEwMTQyMjMzMTBaFw0xNzEwMTMyMjMz
# MTBaMIHCMQswCQYDVQQGEwJBVTEMMAoGA1UECBMDQUNUMREwDwYDVQQHEwhDYW5i
# ZXJyYTFEMEIGA1UEChM7RGVwYXJ0bWVudCBvZiBFZHVjYXRpb24sIEVtcGxveW1l
# bnQgYW5kIFdvcmtwbGFjZSBSZWxhdGlvbnMxJTAjBgNVBAsMHENvbW11bmljYXRp
# b25zICYgSVQgU2VjdXJpdHkxJTAjBgNVBAMTHEVEQyBDb2RlIFNpZ25pbmcgQ2Vy
# dGlmaWNhdGUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCt31qcF5W/
# eJloqfSEeL97v4LXgVF7JcjbfAEHgdFuw0sdbZSDNzZTT+63VQwN99Gn0JUoOjbl
# uMYVDXWpp7EPAKXq0ztuIqs+nTW1CiyMwHRcLF8Uc5dtROoGRglzfJT8yqfRas2L
# egsJWN0oMOFsbVjDDeb1Pxhq9Wutj0OMwdrM0AD7+EMjiuZeKAN6qKSKHiis1d6n
# tsuwAPLcpNG1OL0jYld49WRXEc/AP4cuJ1uRbNCaABSaK8Zaca121ZE1fFHprWIb
# veDIXQ07kWM7q1FBPg27/3QxS6lr4HlCEucZACz7IByd9OaffCdTlhxkhUGsE4pL
# wpfrYgyjK2X9AgMBAAGjggMQMIIDDDALBgNVHQ8EBAMCB4AwRAYJKoZIhvcNAQkP
# BDcwNTAOBggqhkiG9w0DAgICAIAwDgYIKoZIhvcNAwQCAgCAMAcGBSsOAwIHMAoG
# CCqGSIb3DQMHMBMGA1UdJQQMMAoGCCsGAQUFBwMDMB0GA1UdDgQWBBQW5nMXaPSN
# eQkzDAzo+BJxLi1ObTAfBgNVHSMEGDAWgBQ9zZCy977CBjAn9Ed8XRSGoc6nsTCB
# 9gYDVR0fBIHuMIHrMIHooIHloIHihoGobGRhcDovLy9DTj1FRENwcm9kQ0EsQ049
# ZndhcG4xNDIsQ049Q0RQLENOPVB1YmxpYyUyMEtleSUyMFNlcnZpY2VzLENOPVNl
# cnZpY2VzLENOPUNvbmZpZ3VyYXRpb24sREM9cmFkaXg/Y2VydGlmaWNhdGVSZXZv
# Y2F0aW9uTGlzdD9iYXNlP29iamVjdENsYXNzPWNSTERpc3RyaWJ1dGlvblBvaW50
# hjVodHRwOi8vZndhcG4xNDIubmF0aW9uLnJhZGl4L0NlcnRFbnJvbGwvRURDcHJv
# ZENBLmNybDCCAQsGCCsGAQUFBwEBBIH+MIH7MIGfBggrBgEFBQcwAoaBkmxkYXA6
# Ly8vQ049RURDcHJvZENBLENOPUFJQSxDTj1QdWJsaWMlMjBLZXklMjBTZXJ2aWNl
# cyxDTj1TZXJ2aWNlcyxDTj1Db25maWd1cmF0aW9uLERDPXJhZGl4P2NBQ2VydGlm
# aWNhdGU/YmFzZT9vYmplY3RDbGFzcz1jZXJ0aWZpY2F0aW9uQXV0aG9yaXR5MFcG
# CCsGAQUFBzAChktodHRwOi8vZndhcG4xNDIubmF0aW9uLnJhZGl4L0NlcnRFbnJv
# bGwvZndhcG4xNDIubmF0aW9uLnJhZGl4X0VEQ3Byb2RDQS5jcnQwPQYJKwYBBAGC
# NxUHBDAwLgYmKwYBBAGCNxUIhp2mTYb7yV+H8Yskhai0Bv6UUIFWhtehZoGTwTIC
# AWQCAQQwGwYJKwYBBAGCNxUKBA4wDDAKBggrBgEFBQcDAzANBgkqhkiG9w0BAQUF
# AAOCAQEABf0aMwLCOiHJU/YlFC4UB5Fiwjo/ZkBvNt3+CdKRVDptcvbPlCuYUg81
# S02jGvuILef5bvTTSeVM7F92BRGA1U1CvR+l/OZBQwmWVm9Kua0ElXCAWCf+YP2r
# Cu19+65zBRVceoehn4bVjffRO4pBgR/Z3AnYv2qvwN9tRO3YhJ5KAmGbB42JY+5V
# Z/eX0RXvd8f0Nr39z2cYnXoCSzpazi9DHagj+009zErT3YwOqp2D8AXRKdLechbv
# 8wtIAaSDXM9Cpv3hOwONtXs2/emhhzxuzWIVEDz9LCr3cITGHGFevCM3GFc66AA3
# MkoeU60tBvWowwL5Ary5XgNDq53BVjGCAfIwggHuAgEBMFEwQzEVMBMGCgmSJomT
# 8ixkARkWBXJhZGl4MRYwFAYKCZImiZPyLGQBGRYGbmF0aW9uMRIwEAYDVQQDEwlF
# RENwcm9kQ0ECCmGC6IQAAAAAbuMwCQYFKw4DAhoFAKB4MBgGCisGAQQBgjcCAQwx
# CjAIoAKAAKECgAAwGQYJKoZIhvcNAQkDMQwGCisGAQQBgjcCAQQwHAYKKwYBBAGC
# NwIBCzEOMAwGCisGAQQBgjcCARUwIwYJKoZIhvcNAQkEMRYEFDRtEEvVh/JilGlv
# duHTLZ8B2VjQMA0GCSqGSIb3DQEBAQUABIIBAHTYI3JCGlrz2rw/6D+1Lvu6K9RH
# XFMx3go1O0ViN4f7Xn7tH/I+WWJFlW4a5AW9sl6xdj1RdIQaBJEUWncPhjJojiAE
# t3r29rY/6H1TbS8F2nmxhzw0WsDbu520490ZbjU6Oef0pMtxoGHHdEDySLh1pwun
# OHxHoooVGCGdMLraxZXp0qCTDyo06fQtm+tc8yI1fWdX2Tk23E4O4ya9KwTnGRjF
# PluR4PHVtJn0dT7OMcrOR+Wzi+0CId4LrLFgIn8//Dc8jMHMThBqF8AK2G0QqyPl
# kfuZSEIoTSwd6TxFNkJMe3LjZcxTzlemihJC/DZtSiYUoNwfrVcvWWoF/8o=
# SIG # End signature block
