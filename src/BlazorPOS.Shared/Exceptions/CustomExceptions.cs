namespace BlazorPOS.Shared.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }

    public class InsufficientInventoryException : Exception
    {
        public int ProductId { get; }
        public int RequestedQuantity { get; }
        public int AvailableQuantity { get; }

        public InsufficientInventoryException(int productId, int requestedQuantity, int availableQuantity)
            : base($"Insufficient inventory for product {productId}. Requested: {requestedQuantity}, Available: {availableQuantity}")
        {
            ProductId = productId;
            RequestedQuantity = requestedQuantity;
            AvailableQuantity = availableQuantity;
        }
    }

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors) : base("One or more validation errors occurred.")
        {
            Errors = errors;
        }
    }