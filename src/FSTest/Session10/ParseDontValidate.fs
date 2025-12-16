module FSTest.Session10.ParseDontValidate


// products
// id, name, isFood, price, expiryDate

type ProductDb =
    { id: int
      name: string
      isFood: bool
      price: decimal
      expiryDate: System.DateTime }

type BaseProduct =
    { id: int
      name: string
      price: decimal }

type FoodProduct =
    { baseProduct: BaseProduct
      expiryDate: System.DateTime }

type OtherProduct =
    { id: int
      name: string
      price: decimal }

type Product =
    | FoodProduct of FoodProduct
    | OtherProduct of BaseProduct

let readFromDb () : ProductDb list=
    failwith "Not yet implemented"

let applyDiscount (food: FoodProduct list) =
    failwith "Not yet implemented"

let expiryDate (db: ProductDb) =
    db.expiryDate

let toDomainObject (db: ProductDb): Product =
    let baseProduct: BaseProduct =
        { id = db.id
          name = db.name
          price = db.price }

    if db.isFood
    then FoodProduct { baseProduct = baseProduct
                       // expiryDate = expiryDate db }
                       expiryDate = db.expiryDate }
    else
        OtherProduct baseProduct


let repository () =
    let productsDb = readFromDb ()
    productsDb |> List.map (fun p -> toDomainObject p)


let mustBeFood (product: Product) =
    match product with
    | FoodProduct foodProduct -> Some foodProduct
    | OtherProduct _ -> None

let main () =
    let products = repository ()
    let food =
        products
        |> List.choose mustBeFood

    applyDiscount food
