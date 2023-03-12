type CashType = { amount: float }
type CreditCardType = {  amount: float; banckAccount: string }
type MobilePayType = { amount: float; telefoneNumber: string }

type Payment = Cash of CashType | CreditCard of CreditCardType | MobilePay of MobilePayType

