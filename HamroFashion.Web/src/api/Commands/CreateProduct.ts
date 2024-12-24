export default interface CreateProduct {
    name: string;
    description?: string;
    color: string;
    imageUrl: string;
    price: string;
    size: string;
    quantity: string;
    availability: boolean;
    productCategory: string;
    productCollection: string[];
    productLabel: string[];
}