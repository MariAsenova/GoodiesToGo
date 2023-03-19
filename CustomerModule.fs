module CustomerM

type VIAPersonType = { viaId: string }
type SOSUPersonType = { sosuId: string }

type Customer = VIAPerson of VIAPersonType | SOSUPerson of SOSUPersonType
