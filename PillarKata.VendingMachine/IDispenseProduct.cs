namespace PillarKata.VendingMachine
{
    /// <summary>
    /// In a real vending machine we would delegate dispensing to a mechanism. This is the interface to its firmware.
    /// </summary>
    public interface IDispenseProduct
    {
        void DispenseProduct(string productCode);
    }
}