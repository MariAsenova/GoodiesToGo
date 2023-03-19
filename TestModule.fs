module TestM
open ProductM
open PaymentM
open OrderM

// Create individual products
let product1 = Product.Drink(DrinkBase.Coffee { size = Size.Small; coffeeType = CoffeeType.Espresso }, 1)
let product2 = Product.Drink(DrinkBase.Coffee { size = Size.Large; coffeeType = CoffeeType.Americano }, 1)
let product3 = Product.Drink(DrinkBase.Tea { size = Size.Medium; teaType = TeaType.Green }, 2)
let product5 = Product.Drink(DrinkBase.Juice { size = Size.Large; juiceType = JuiceType.Orange }, 10)
let product8 = Product.Drink(DrinkBase.Coffee { size = Size.Medium; coffeeType = CoffeeType.Latte }, 25)

// Create a list of products for the order
let products = [ product1 ; product2 ; product3 ; product4 ; product5 ; product6 ; product7 ]

// Create a payment method for the order
let payment = Payment.CreditCard { bankAccount = "DK00559319"; amount = 0.0 }

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
