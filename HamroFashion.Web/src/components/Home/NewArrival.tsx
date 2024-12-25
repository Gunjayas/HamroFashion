import React, { useState, useEffect, TouchEvent } from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';

interface Product {
  id: number;
  name: string;
  image: string;
}

const NewArrival: React.FC = () => {
  const products: Product[] = [
    
  ];

  const [currentIndex, setCurrentIndex] = useState<number>(0);
  const [isAnimating, setIsAnimating] = useState<boolean>(false);
  const [touchStart, setTouchStart] = useState<number | null>(null);
  const [touchEnd, setTouchEnd] = useState<number | null>(null);

  // Auto-scroll functionality
  useEffect(() => {
    const timer = setInterval(() => {
      handleNext();
    }, 5000);

    return () => clearInterval(timer);
  }, [currentIndex]);

  const handlePrev = () => {
    if (!isAnimating) {
      setIsAnimating(true);
      setCurrentIndex((prev) => (prev === 0 ? products.length - 1 : prev - 1));
      setTimeout(() => setIsAnimating(false), 500);
    }
  };

  const handleNext = () => {
    if (!isAnimating) {
      setIsAnimating(true);
      setCurrentIndex((prev) => (prev === products.length - 1 ? 0 : prev + 1));
      setTimeout(() => setIsAnimating(false), 500);
    }
  };

  // Touch handlers for mobile swipe
  const handleTouchStart = (e: TouchEvent<HTMLDivElement>) => {
    setTouchStart(e.targetTouches[0].clientX);
  };

  const handleTouchMove = (e: TouchEvent<HTMLDivElement>) => {
    setTouchEnd(e.targetTouches[0].clientX);
  };

  const handleTouchEnd = () => {
    if (!touchStart || !touchEnd) return;

    const distance = touchStart - touchEnd;
    const isLeftSwipe = distance > 50;
    const isRightSwipe = distance < -50;

    if (isLeftSwipe) {
      handleNext();
    }
    if (isRightSwipe) {
      handlePrev();
    }

    setTouchStart(null);
    setTouchEnd(null);
  };

  const getVisibleProducts = (): Product[] => {
    const visibleCount = window.innerWidth >= 1024 ? 4 : window.innerWidth >= 768 ? 2 : 1;
    const items: Product[] = [];
    for (let i = 0; i < visibleCount; i++) {
      const index = (currentIndex + i) % products.length;
      items.push(products[index]);
    }
    return items;
  };

  return (
    <div className="bg-white py-10 px-4 sm:px-7">
      <div className="container mx-auto">
        {/* Section Title */}
        <h2 className="text-2xl font-bold mb-6 md:ml-12 text-center md:text-left">
          New Arrival
        </h2>

        {/* Carousel */}
        <div className="relative">
          {/* Left Arrow */}
          <button
            onClick={handlePrev}
            className="absolute left-0 top-1/2 -translate-y-1/2 -translate-x-2 md:-translate-x-4 z-10 
                     text-2xl p-2 rounded-full border border-gray-300 bg-white shadow-lg
                     hover:bg-gray-50 active:bg-gray-100 transition-all duration-300
                     focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <ChevronLeft className="w-6 h-6" />
          </button>

          {/* Products List */}
          <div
            className="overflow-hidden mx-8"
            onTouchStart={handleTouchStart}
            onTouchMove={handleTouchMove}
            onTouchEnd={handleTouchEnd}
          >
            <div
              className="flex transition-transform duration-500 ease-in-out gap-4 md:gap-6"
              style={{
                transform: `translateX(-${currentIndex * (100 / getVisibleProducts().length)}%)`,
              }}
            >
              {products.map((product) => (
                <div
                  key={product.id}
                  className="w-full md:w-1/2 lg:w-1/4 flex-shrink-0 group cursor-pointer"
                >
                  <div className="flex flex-col items-center transform transition-transform duration-300 hover:scale-105">
                    <div className="relative w-full overflow-hidden rounded-lg">
                      <img
                        src={product.image}
                        alt={product.name}
                        className="w-full h-64 object-cover transform transition-transform duration-300 group-hover:scale-110"
                      />
                      <div className="absolute inset-0 bg-black opacity-0 group-hover:opacity-10 transition-opacity duration-300" />
                    </div>
                    <h3 className="text-lg font-semibold mt-4 group-hover:text-blue-600 transition-colors duration-300">
                      {product.name}
                    </h3>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Right Arrow */}
          <button
            onClick={handleNext}
            className="absolute right-0 top-1/2 -translate-y-1/2 translate-x-2 md:translate-x-4 z-10 
                     text-2xl p-2 rounded-full border border-gray-300 bg-white shadow-lg
                     hover:bg-gray-50 active:bg-gray-100 transition-all duration-300
                     focus:outline-none focus:ring-2 focus:ring-blue-500"
          >
            <ChevronRight className="w-6 h-6" />
          </button>

          {/* Dots Navigation */}
          <div className="flex justify-center gap-2 mt-6">
            {products.map((_, index) => (
              <button
                key={index}
                className={`w-2 h-2 rounded-full transition-all duration-300 ${
                  index === currentIndex ? 'bg-blue-600 w-4' : 'bg-gray-300'
                }`}
                onClick={() => setCurrentIndex(index)}
              />
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};

export default NewArrival;
