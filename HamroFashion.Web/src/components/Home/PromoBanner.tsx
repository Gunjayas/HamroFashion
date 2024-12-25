import React from "react";

const Promobanner: React.FC = () => {
  return (
    <div className="container mx-auto px-4 mb-16">
      <div className="relative bg-black text-white rounded-xl overflow-hidden 
                    transform transition-all duration-500 hover:shadow-2xl">
        
        {/* Background Pattern */}
        <div className="absolute inset-0 opacity-20">
          <div className="absolute inset-0 bg-gradient-to-r from-purple-600 to-pink-600 
                         animate-gradient"></div>
        </div>

        <div className="flex flex-col lg:flex-row items-center relative z-10">
          {/* Image Container */}
          <div className="w-full lg:w-1/2 relative overflow-hidden">
            <div className="absolute inset-0 bg-black opacity-50"></div>
            <img
              src="./promobannerimage.png"
              alt="Promo"
              className="w-full h-[300px] lg:h-[400px] object-cover object-center 
                         transform transition-all duration-700 hover:scale-110"
            />
          </div>

          {/* Content */}
          <div className="w-full lg:w-1/2 flex flex-col justify-center p-8 lg:p-12 
                         animate-slideIn">
            <div className="max-w-lg mx-auto lg:mx-0">
              <h1 className="text-2xl md:text-3xl lg:text-4xl font-bold mb-6 
                            animate-fadeIn
                            transform transition-all duration-300 hover:scale-105">
                <span className="inline-block animate-shimmer bg-gradient-to-r from-white via-purple-200 to-white 
                               bg-clip-text text-transparent">
                  GET 20% OFF
                </span>
                <br />
                <span className="inline-block mt-2">ON YOUR FIRST DEAL</span>
              </h1>

              <button className="group relative inline-flex items-center justify-center
                               px-6 py-3 mt-6 w-auto
                               bg-transparent border-2 border-white text-white
                               rounded-full overflow-hidden
                               transition-all duration-300
                               hover:bg-white hover:text-black
                               transform hover:scale-105
                               focus:outline-none focus:ring-2 focus:ring-white focus:ring-opacity-50">
                <span className="relative z-10 font-semibold">
                  Order Now
                </span>
                <div className="absolute inset-0 bg-white transform scale-x-0 group-hover:scale-x-100 
                               transition-transform duration-500 origin-left"></div>
              </button>
            </div>
          </div>
        </div>
      </div>

      {/* Custom Animations */}
      <style>{`
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

        @keyframes shimmer {
          0% {
            background-position: -200% center;
          }
          100% {
            background-position: 200% center;
          }
        }

        @keyframes gradient {
          0% {
            background-position: 0% 50%;
          }
          50% {
            background-position: 100% 50%;
          }
          100% {
            background-position: 0% 50%;
          }
        }

        .animate-slideIn {
          animation: slideIn 1s ease-out forwards;
        }

        .animate-fadeIn {
          animation: fadeIn 0.8s ease-out forwards;
        }

        .animate-shimmer {
          animation: shimmer 3s linear infinite;
          background-size: 200% auto;
        }

        .animate-gradient {
          animation: gradient 15s ease infinite;
          background-size: 400% 400%;
        }
      `}</style>
    </div>
  );
};

export default Promobanner;
