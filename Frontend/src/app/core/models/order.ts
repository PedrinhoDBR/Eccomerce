export interface OrderItem {
  productId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  subtotal: number;
}

export interface Order {
  id: string;
  customerName: string;
  customerEmail: string;
  address: string;
  phone: string;
  createdAt: string;
  status: string;
  total: number;
  items: OrderItem[];
}
