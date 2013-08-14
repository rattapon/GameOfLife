namespace GameOfLife.FSharp

open System

module GameOfLife = 

    [<CompiledName("Initialize")>]
    let initialize role column = 
        if role < 1 || column < 1 then raise (System.ArgumentException("Cannot initialize with size that is smaller than 1"))
        Array2D.init role column (fun _ _ -> false)
        
    [<CompiledName("LiveCell")>]
    let wakeUpCell coordinates grid  = 
        Array2D.mapi (fun x y cell -> coordinates |> Seq.exists ((=) (x,y)) || cell) grid

    let isCellAlive x y grid = 
        if x < 0 || x >= Array2D.length1 grid ||
            y < 0 || y >= Array2D.length2 grid then
            0
        else
            if grid.[x, y] then 1 else 0
            
    [<CompiledName("CountNeighbours")>]
    let countNeighbours x y grid = 
        let topLeft = isCellAlive (x-1) (y+1) grid
        let topMid = isCellAlive x (y+1) grid
        let topRight = isCellAlive (x+1) (y+1) grid
        let left = isCellAlive (x-1) y grid
        let right = isCellAlive (x+1) y grid
        let bottomLeft = isCellAlive (x-1) (y-1) grid
        let bottomMid = isCellAlive x (y-1) grid
        let bottomRight = isCellAlive (x+1) (y-1) grid
        topLeft + topMid + topRight + left + right + bottomLeft + bottomMid + bottomRight

    [<CompiledName("NextGeneration")>]
    let processNextGen grid = 
        let row = Array2D.length1 grid
        let column = Array2D.length2 grid
        Array2D.init row column (fun x y ->
            let numNeighbours = countNeighbours x y grid
            match numNeighbours with
            | 2 -> grid.[x, y]
            | 3 -> true
            | _ -> false)