module ProductM
open System

// Helper function to calculate price with VAT
let applyVAT (percentage : int) (price : float) = 
    price * (1.0 + Convert.ToDouble(percentage) / 100.0)

// Types for different sizes and drink varieties
type Size = Small | Medium | Large
type CoffeeType = Americano | Latte | Espresso
type TeaType = Green | Black | Jasmine
type JuiceType = Apple | Orange | Grepefruit

// Record types for drinks with their size and type
type CoffeeRecord = {size: Size; coffeeType: CoffeeType}
type TeaRecord = {size: Size; teaType: TeaType}
type JuiceRecord = {size: Size; juiceType: JuiceType}

// Union type for different drink categories
type DrinkBase = Coffee of CoffeeRecord | Tea of TeaRecord | Juice of JuiceRecord

// Functions to calculate the price based on the size of each drink type
let priceSizeCoffee (coffee : CoffeeRecord) = 
    match coffee.size with 
    | Small -> 8.0
    | Medium -> 10.0
    | Large -> 12.0

let priceSizeTea (tea : TeaRecord) = 
    match tea.size with 
    | Small -> 6.0
    | Medium -> 8.0
    | Large -> 10.0

let priceSizeJuice (juice : JuiceRecord) = 
    match juice.size with 
    | Small -> 10.0
    | Medium -> 12.0
    | Large -> 14.0

// Function to calculate the price of a drink matching type
let priceDrink (drink : DrinkBase) = 
    match drink with 
    | Coffee(coffeeRecord) -> priceSizeCoffee(coffeeRecord) |> applyVAT 25
    | Tea(teaRecord) -> priceSizeTea(teaRecord)
    | Juice(juiceRecord) -> priceSizeJuice(juiceRecord)

// Union type for products, which can be drinks with quantity
type Product = 
    | Drink of DrinkBase * qty: int 

// Function to calculate the price of a product
let calculatePrice (product: Product) = 
    match product with
    | Drink(drink, quantity) -> priceDrink(drink) * float quantity

// Function to calculate the total price of a list with products
let calculatePriceTotal (products: Product list) = 
    let rec calcTotalRec pr acc = 
        match pr with
        | [] -> acc
        | hd::tl -> (calculatePrice hd) + acc |> calcTotalRec tl
    calcTotalRec products 0.0

(*
    This function is a recursive function that calculates the total price of a list with products. The calcTotalRec function is defined within the 
    calculatePriceTotal function, which takes two arguments, pr and acc. pr is a list of products, and acc is the accumulated total price. 
    It uses pattern matching to check if the list is empty or has elements. If the list is empty, it returns the accumulated total defaulted to 0.0. 
    If there are elements, it adds the price of the first product to the accumulated total and calls the function recursively with the tail of the 
    list. Finally, calculatePriceTotal calls calcTotalRec with the initial products list and an initial accumulated total of 0.0
    *) 
