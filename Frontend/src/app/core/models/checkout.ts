export interface CheckoutItemRequest {
  productId: string;
  quantity: number;
}

export interface CheckoutRequest {
  customerName: string;
  customerEmail: string;
  items: CheckoutItemRequest[];
}
