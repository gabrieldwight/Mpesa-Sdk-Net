namespace MpesaSdk.Enums
{
    /// <summary>
    /// This is an identity of a M-Pesa transaction’s sending and receiving party as either a shortcode, a till number or a MSISDN (phone number).
    /// </summary>
    public enum IdentifierTypes
    {
        MSISDN = 1,
        TillNumber = 2,
        Shortcode = 4,
    }
}
