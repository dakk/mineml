
namespace MineML
open System

type Utils() = 
    static member toBytes (input : string) =
        let mutable bytes : byte [] = Array.create (input.Length / 2) (new Byte())
        let mutable i = 0
        let mutable j = 0
        while i < input.Length do
            bytes.SetValue (Byte.Parse(input.Substring(i, 2), System.Globalization.NumberStyles.HexNumber), j)
            i <- i + 2
            j <- j + 1
        bytes
        
        
    static member toStringFromByte (input : byte []) =
        let rec tostr res (bl : byte list) =
            match bl with
                | [] -> res
                | b::xl -> tostr (res+b.ToString("x2")) xl
                
        tostr "" (List.ofArray input)


    static member toStringFromUint (value : uint32) =
        toStringFromByte (BitConverter.GetBytes(value))
        
        
    static member endianFlip32BitChunks (input : string) =
        let mutable res = ""
        let chs = input.ToCharArray ()
        
        for i in 0 .. 8 .. input.Length do
            for j in 0 .. 2 .. 8 do
                res <- res + ((chs.GetValue (i - j + 6)).ToString ())
                res <- res + ((chs.GetValue (i - j + 7)).ToString ())
            
        res
    
    
    static member removePadding (input : string) = 
        input.Substring(0, 160)
    
    
    static member addPadding (input : string) =
        input + "000000800000000000000000000000000000000000000000000000000000000000000000000000000000000080020000"
        
        