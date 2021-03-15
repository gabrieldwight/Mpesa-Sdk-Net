namespace MpesaSdk.Enums
{
    /// <summary>
    /// This is codes obtained from the Mpesa Results.
    /// </summary>
    public enum MpesaResultCode
    {
        Success = 0,
        InsufficientFunds = 1,
        LessThanMinimumTransactionValue = 2,
        MoreThanMaximumTransactionValue = 3,
        WouldExceedDailyTransferLimit = 4,
        WouldExceedMinimumBalance = 5,
        UnresolvedPrimaryParty = 6,
        UnresolvedReceiverParty = 7,
        WouldExceedMaximumBalance = 8,
        DebitAccountInvalid = 11,
        CreditAccountInvalid = 12,
        UnresolvedDebitAccount = 13,
        UnresolvedCreditAccount = 14,
        DuplicateDetected = 15,
        InternalFailure = 17,
        UnresolvedInitiator = 20,
        TrafficBlockingConditionInPlace = 26
    }
}
