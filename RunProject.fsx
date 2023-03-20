open System

(*
    Product Moduel 
*)

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

(*
    Payment Module
*)

type CashType = { amount: float }
type CreditCardType = {  amount: float; banckAccount: string }
type MobilePayType = { amount: float; telefoneNumber: string }

type Payment = Cash of CashType | CreditCard of CreditCardType | MobilePay of MobilePayType

(*
    Order Module
*)

// Record type for an order containing a list of products and a payment method
type OrderRecord = { products: Product List; payment: Payment }

// Function to print the payment details based on the payment method
let printPayment payment total = 
    match payment with
    | Cash(cashR) -> 
        printfn "The order total %f has been fully paid in cash." total
    | CreditCard(ccR) ->
        printfn "The order total %f has been fully paid using a credit card from account %s." total ccR.banckAccount
    | MobilePay(mpR) ->
        printfn "The order total %f has been fully paid using MobilePay %s." total mpR.telefoneNumber


// Function to calculate the total price of an order
let orderTotal (order: OrderRecord) = calculatePriceTotal order.products

// Function to process an order and print the payment details
let payOrder (order: OrderRecord) = 
    let total = orderTotal order
    printPayment order.payment total

// Union type for messages that can be sent to the agent
type OrderProductMsg = 
    | Order of OrderRecord 
    | LeaveAComment of string

// Agent to handle incoming messages for processing orders and leaving comments
let orderAgent = MailboxProcessor<OrderProductMsg>.Start(fun inbox -> 
    let rec processMessages = async {
        let! msg = inbox.Receive()
        match msg with 
        | Order(orderRecord) -> payOrder orderRecord // Process the order and print payment details
        | LeaveAComment(comment) -> printfn "%s" comment // Print the comment
        return! processMessages // Continue processing messages
    }
    processMessages)

    (*
        Customer Module
    *)

type VIAPersonType = { viaId: string }
type SOSUPersonType = { sosuId: string }

type Customer = VIAPerson of VIAPersonType | SOSUPerson of SOSUPersonType

(*
    Test Module
*)

// Create individual products
let product1 = Product.Drink(DrinkBase.Coffee { size = Size.Small; coffeeType = CoffeeType.Espresso }, 1)
let product2 = Product.Drink(DrinkBase.Coffee { size = Size.Large; coffeeType = CoffeeType.Americano }, 1)
let product3 = Product.Drink(DrinkBase.Tea { size = Size.Medium; teaType = TeaType.Green }, 2)
let product4 = Product.Drink(DrinkBase.Juice { size = Size.Large; juiceType = JuiceType.Orange }, 1)
let product5 = Product.Drink(DrinkBase.Coffee { size = Size.Medium; coffeeType = CoffeeType.Latte }, 1)

// Create a list of products for the order
let products = [ product1 ; product2 ; product3 ; product4 ; product5]

// Create a payment method for the order
let payment = Payment.CreditCard { banckAccount = "DK00559319"; amount = 0.0 }

// Create an order record with the products and payment method
let order = { products = products; payment = payment }

// Call the payOrder function from the OrderM module to process the order and print payment details
payOrder order

// Send the order message to the order agent from the OrderM module
let orderMsg = OrderProductMsg.Order order
orderAgent.Post orderMsg

// Send a comment message to the order agent from the OrderM module
let comment = "My latte did not have enough milk"
let commentMsg = OrderProductMsg.LeaveAComment comment
orderAgent.Post commentMsg
