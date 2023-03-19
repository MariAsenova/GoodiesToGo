module Product

// types of drinks using discriminated union

type DrinkSize =
    | Small
    | Medium
    | Large

type CoffeeType =
    | Americano
    | Espresso
    | Latte

type TeaType =
    | Green
    | Black
    | Jasmine


type JuiceType =
    | Orange
    | Apple
    | Grepefruit

type CoffeeDrink = { drinkSize:DrinkSize; coffeeType:CoffeeType }
type TeaDrink = {drinkSize:DrinkSize; teaType: TeaType}
type JuiceDrink = {drinkSize:DrinkSize; juiceType: JuiceType}

type DrinkType =
    | CoffeeDrink
    | TeaDrink
    | JuiceDrink

type Drink =
    { DrinkType : DrinkType
      DrinkSize : DrinkSize }

let priceForDrink (drink: Drink) =
    match drink with
    | { DrinkType = CoffeeDrink; DrinkSize = Small } -> 8
    | { DrinkType = CoffeeDrink; DrinkSize = Medium } -> 10
    | { DrinkType = CoffeeDrink; DrinkSize = Large } -> 12
    | { DrinkType = TeaDrink; DrinkSize = Small } -> 6
    | { DrinkType = TeaDrink; DrinkSize = Medium } -> 8
    | { DrinkType = TeaDrink; DrinkSize = Large } -> 10
    | { DrinkType = JuiceDrink; DrinkSize = Small } -> 10
    | { DrinkType = JuiceDrink; DrinkSize = Medium } -> 12
    | { DrinkType = JuiceDrink; DrinkSize = Large } -> 14
    | _ -> failwith "Invalid drink size or type. Please, reenter your order."

