namespace PillarKata.VendingMachine.Tests
{

    public class StubbedDispenser : IDispenseProduct
    {
        public StubbedDispenser()
        {
            DispensedProduct = "";
        }

        public bool WasProductDispensed
        {
            get { return !string.IsNullOrEmpty(DispensedProduct); }
        }

        public string DispensedProduct { get; private set; }

        public void DispenseProduct(string productCode)
        {
            DispensedProduct = productCode;
        }
    }
}