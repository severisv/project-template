namespace HappyBever

module Nullable =    
    let toOption (n : System.Nullable<_>) = 
           if n.HasValue 
           then Some n.Value 
           else None

    let map (f : 'a -> 'b) (n : System.Nullable<_>) = 
               if n.HasValue 
               then Some <| f n.Value 
               else None       
