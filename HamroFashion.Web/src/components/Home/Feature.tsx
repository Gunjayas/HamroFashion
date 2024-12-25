import React from 'react';

interface StyleProps {
  fadeIn: string;
  slideIn: string;
}

const Feature: React.FC = () => {
  // Define animation styles
  const animationStyles: StyleProps = {
    fadeIn: `
      @keyframes fadeIn {
        from {
          opacity: 0;
          transform: translateY(20px);
        }
        to {
          opacity: 1;
          transform: translateY(0);
        }
      }
    `,
    slideIn: `
      @keyframes slideIn {
        from {
          opacity: 0;
          transform: translateX(50px);
        }
        to {
          opacity: 1;
          transform: translateX(0);
        }
      }
    `
  };

  return (
    <div className="container mx-auto px-4 mb-20">
      <div className="flex flex-col lg:flex-row items-center justify-center gap-6 bg-white rounded-xl overflow-hidden shadow-lg transform hover:scale-[1.02] transition-all duration-500">
        {/* Left Side - Text Content */}
        <div 
          className="w-full lg:w-1/2 min-h-[400px] p-4 md:p-8 bg-cover bg-center relative group"
          style={{ backgroundImage: 'url("./Feature-section/cover-feature.png")' }}
        >
          {/* Overlay with animation */}
          <div className="absolute inset-0 bg-black bg-opacity-40 group-hover:bg-opacity-60 transition-all duration-500"></div>
          
          {/* Content Container */}
          <div className="relative h-full flex flex-col justify-center">
            <div className="bg-black bg-opacity-50 backdrop-blur-sm p-6 md:p-8 rounded-xl 
                          transform transition-all duration-500 
                          hover:translate-x-2 hover:shadow-2xl
                          opacity-0 animate-fadeIn">
              <h2 className="text-white text-2xl md:text-3xl lg:text-4xl font-bold mb-4 
                           transform transition-all duration-500
                           translate-y-4 group-hover:translate-y-0">
                YOUR OUTFIT EXPRESS WHO YOU ARE
              </h2>
              <p className="text-gray-300 mb-6 text-sm md:text-base
                           transform transition-all duration-500 delay-100
                           translate-y-4 group-hover:translate-y-0">
                Fashion is a way for people to express themselves and show their independence 
                during a specific time period.
              </p>
              <button 
                className="bg-white text-black py-2 px-6 rounded-full font-semibold
                          transform transition-all duration-300
                          hover:bg-purple-600 hover:text-white
                          hover:scale-105 hover:shadow-lg
                          focus:outline-none focus:ring-2 focus:ring-purple-500
                          active:scale-95"
                onClick={() => {
                  // Add type-safe click handler if needed
                  console.log('Explore button clicked');
                }}
              >
                Explore Now
              </button>
            </div>
          </div>
        </div>

        {/* Right Side - Image */}
        <div className="w-full lg:w-1/2 overflow-hidden">
          <img
            className="w-full h-[400px] object-cover object-center 
                       transform transition-all duration-700
                       hover:scale-110 
                       animate-slideIn"
            src="./Feature-section/spider-gf.png"
            alt="Zendaya"
          />
        </div>
      </div>

      {/* Animations */}
      <style>
        {`
          ${animationStyles.fadeIn}
          ${animationStyles.slideIn}

          .animate-fadeIn {
            animation: fadeIn 1s ease-out forwards;
          }

          .animate-slideIn {
            animation: slideIn 1s ease-out forwards;
          }
        `}
      </style>
    </div>
  );
};

export default Feature;