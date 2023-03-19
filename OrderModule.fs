module OrderM

open ProductM


// Record type for an order containing a list of products and a payment method
type OrderRecord = { products: Product List; payment: Payment }

// Function to print the payment details based on the payment method
let printPayment payment total = 
    match payment with
    | Cash(cashR) -> 
        printfn "The order total %f has been fully paid in cash." total
    | CreditCard(ccR) ->
        printfn "The order total %f has been fully paid using a credit card from account %s." total ccR.bankAccount

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
        | Order(OrderRecord) -> payOrder OrderRecord // Process the order and print payment details
        | LeaveAComment(comment) -> printfn "%s" comment // Print the comment
        return! processMessages // Continue processing messages
    }
    processMessages)

