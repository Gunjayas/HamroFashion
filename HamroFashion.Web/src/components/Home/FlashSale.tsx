import React from 'react';
import { StarIcon } from 'lucide-react';
import { Heart, ShoppingCart } from 'lucide-react';

interface Product {
  id: number;
  name: string;
  brand: string;
  price: string;
  rating: number;
  imageUrl: string;
}

const products: Product[] = [
  {
    id: 1,
    name: 'Black Jacket',
    brand: 'Patagonia Brand',
    price: 'NPR 699',
    rating: 4.2,
    imageUrl: './Flash-sale-section/model-1.png',
  },
  {
    id: 2,
    name: 'Trendy Summer Top',
    brand: "AS's Brand",
    price: 'NPR 499',
    rating: 4.5,
    imageUrl: './Flash-sale-section/model-2.png',
  },
  {
    id: 3,
    name: 'Jeans Pants',
    brand: "MM's Brand",
    price: 'NPR 999',
    rating: 5.0,
    imageUrl: './Flash-sale-section/model-3.png',
  },
  {
    id: 4,
    name: 'Hoodie',
    brand: "Nike's Brand",
    price: 'NPR 1800',
    rating: 3.5,
    imageUrl: './Flash-sale-section/model-4.png',
  },
];

const Flashsale: React.FC = () => {
  const handleWishlistClick = (productId: number): void => {
    // Implement wishlist functionality
    console.log('Added to wishlist:', productId);
  };

  const handleCartClick = (productId: number): void => {
    // Implement cart functionality
    console.log('Added to cart:', productId);
  };

  return (
    <div className="p-4 md:p-6 bg-white rounded-lg">
      <div className="flex flex-col sm:flex-row justify-between items-center mb-6 gap-4">
        <h2 className="text-xl md:text-2xl font-bold flex items-center group">
          <span className="inline-block animate-bounce">⚡</span>
          <span className="group-hover:text-purple-600 transition-colors duration-300">
            Flash Sale
          </span>
        </h2>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 mb-16">
        {products.map((product) => (
          <div
            key={product.id}
            className="bg-white rounded-lg relative transform transition-all duration-300 hover:-translate-y-2 hover:shadow-xl"
          >
            <div className="relative overflow-hidden group">
              <img
                className="w-full h-80 object-cover rounded-t-lg transform transition-transform duration-700 group-hover:scale-110"
                src={product.imageUrl}
                alt={product.name}
              />
              <div className="absolute top-2 right-2 flex flex-col gap-2 opacity-0 group-hover:opacity-100 transition-opacity duration-300">
                <button 
                  className="bg-white rounded-full p-2 shadow hover:bg-purple-50 transition-colors duration-300"
                  onClick={() => handleWishlistClick(product.id)}
                >
                  <Heart className="w-5 h-5 text-gray-600 hover:text-red-500 transition-colors duration-300" />
                </button>
                <button 
                  className="bg-white rounded-full p-2 shadow hover:bg-purple-50 transition-colors duration-300"
                  onClick={() => handleCartClick(product.id)}
                >
                  <ShoppingCart className="w-5 h-5 text-gray-600 hover:text-purple-500 transition-colors duration-300" />
                </button>
              </div>
              <div className="absolute bottom-2 left-2 bg-white text-purple-500 rounded-full px-3 py-1 flex items-center shadow-md transform transition-transform duration-300 hover:scale-105">
                <StarIcon className="w-4 h-4 mr-1" />
                <span className="text-sm font-medium">{product.rating}</span>
              </div>
            </div>

            <div className="p-4">
              <div className="flex justify-between items-start gap-2">
                <div className="flex-1">
                  <h3 className="text-lg font-semibold hover:text-purple-600 transition-colors duration-300">
                    {product.name}
                  </h3>
                  <p className="text-sm text-gray-500">{product.brand}</p>
                </div>
                <div className="text-sm font-bold bg-slate-100 rounded-md p-2 hover:bg-purple-100 transition-colors duration-300">
                  {product.price}
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
};

export default Flashsale;