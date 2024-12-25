import React from 'react'
import { Link } from 'react-router-dom'
import Flashsale from './FlashSale';
import Feature from './Feature';
import Promobanner from './PromoBanner';
import NewArrival from './NewArrival';

interface PolicyItem {
  icon: string;
  title: string;
  desc: string;
}

const HomePage: React.FC = () => {
  const policyItems: PolicyItem[] = [
    { icon: "./shipping-logo.svg", title: "Free Shipping", desc: "On orders over NRP 5000" },
    { icon: "./return-logo.svg", title: "Return Policy", desc: "14 days return" },
    { icon: "./nepal-map.svg", title: "Nepali delivery", desc: "Free delivery within 7 days" },
    { icon: "./refund.svg", title: "Refund Policy", desc: "60 days return for any reason" }
  ];

  return (
    <>
      {/* Hero-section  */}
      <div className="mx-2 sm:mx-4 md:mx-6 lg:mx-auto max-w-7xl">
        <div className="bg-purple-600 rounded-3xl overflow-hidden relative mx-8">
          <div className="flex flex-col lg:flex-row items-center p-6 md:p-8 lg:p-12">
            {/* Left Content Section */}
            <div className="w-full lg:w-1/2 text-white z-10 animate-fade-in-left">
              <h1 className="text-3xl sm:text-4xl md:text-5xl font-bold mb-4 lg:mb-6 animate-slide-up">
                Discover the Latest Collection of Fashion
              </h1>
              <p className="text-base sm:text-lg mb-6 opacity-90 animate-fade-in transition-opacity delay-300">
                Stay ahead of the fashion game with our new arrivals. Explore the latest styles and find your perfect look.
              </p>
              <button className="bg-black text-white px-8 py-3 rounded-full hover:bg-gray-800 
                transition-all duration-300 hover:scale-105 animate-bounce-subtle">
                <Link to="/combos">SHOP NOW</Link>
              </button>
              <div className="flex flex-col sm:flex-row items-center sm:items-start gap-4 sm:gap-0 mt-4 animate-fade-in transition-opacity delay-500">
                <div className="flex -space-x-2 hover:space-x-1 transition-all duration-300">
                  <img className="w-8 sm:w-11 h-8 sm:h-11 rounded-full border-2 border-blue-600 transition-transform hover:scale-110" src="./Home-section/customer-1.png" alt="Customer 1" />
                  <img className="w-8 sm:w-11 h-8 sm:h-11 rounded-full border-2 border-blue-600 transition-transform hover:scale-110" src="./Home-section/customer-2.png" alt="Customer 2" />
                  <img className="w-8 sm:w-11 h-8 sm:h-11 rounded-full border-2 border-blue-600 transition-transform hover:scale-110" src="./Home-section/customer-3.png" alt="Customer 3" />
                </div>
                <span className="ml-0 sm:ml-3">
                  <pre className='font-sans text-2xl sm:text-3xl font-bold animate-count-up'>100+</pre>
                  <span className="text-sm sm:text-base">Happy Customer</span>
                </span>
              </div>
            </div>
            
           {/* Right Image Section */}
           <div className="w-full lg:w-1/2 mt-8 lg:mt-0 relative animate-fade-in-right">
              <div className="relative w-full h-[400px] lg:h-[600px] bg-pink-300 rounded-full overflow-hidden">
                <img
                  src="./Home-section/boy-girl.png"
                  alt="Fashion Models"
                  className="w-full h-full object-cover object-center transition-transform hover:scale-105 duration-500"
                />
              </div>
            </div>
            
            {/* Decorative Stars */}
            {[...Array(12)].map((_, i) => (
              <div
                key={i}
                className="absolute text-yellow-300 animate-float"
                style={{
                  top: `${Math.random() * 100}%`,
                  left: `${Math.random() * 100}%`,
                  animation: `float ${3 + i * 0.5}s ease-in-out infinite ${i * 0.2}s`,
                  fontSize: `${Math.random() * 10 + 20}px`,
                }}
              >
                ★
              </div>
            ))}
          </div>
        </div>
      </div>

      <style>{`
        @keyframes float {
          0%, 100% { transform: translateY(0); }
          50% { transform: translateY(-20px); }
        }

        @keyframes bounce-subtle {
          0%, 100% { transform: translateY(0); }
          50% { transform: translateY(-5px); }
        }

        .animate-bounce-subtle {
          animation: bounce-subtle 2s ease-in-out infinite;
        }

        .animate-fade-in-left {
          animation: fadeInLeft 1s ease-out forwards;
        }

        .animate-fade-in-right {
          animation: fadeInRight 1s ease-out forwards;
        }

        .animate-slide-up {
          animation: slideUp 0.8s ease-out forwards;
        }

        .animate-float {
          animation: float 3s ease-in-out infinite;
        }

        @keyframes fadeInLeft {
          from {
            opacity: 0;
            transform: translateX(-20px);
          }
          to {
            opacity: 1;
            transform: translateX(0);
          }
        }

        @keyframes fadeInRight {
          from {
            opacity: 0;
            transform: translateX(20px);
          }
          to {
            opacity: 1;
            transform: translateX(0);
          }
        }

        @keyframes slideUp {
          from {
            opacity: 0;
            transform: translateY(20px);
          }
          to {
            opacity: 1;
            transform: translateY(0);
          }
        }
      `}</style>


      {/* policy section  */}
      <div className='bg-white rounded-2xl p-4 mx-2 sm:mx-8 md:mx-16 lg:mx-24 mb-8 mt-8 lg:mt-14 hover:shadow-xl transition-shadow duration-300 animate-slide-up'>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 lg:gap-4">
          {policyItems.map((item, index) => (
            <div key={index} className="flex flex-row items-center justify-center sm:justify-start gap-4 hover:scale-105 transition-transform duration-300 p-4">
              <img 
                src={item.icon} 
                alt={item.title} 
                className="w-8 sm:w-10 transition-transform hover:rotate-12 duration-300" 
              />
              <div className='flex flex-col justify-center items-start'>
                <h2 className="text-lg sm:text-xl font-bold">{item.title}</h2>
                <p className="text-sm sm:text-base text-gray-500">{item.desc}</p>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Other sections */}
      <div className='px-2 sm:px-4 md:px-6'>
        <Flashsale/>
        <Feature/>
        <Promobanner/>
        <NewArrival/>
      </div>
    </>
  )
}

// Define global styles type
declare module 'react' {
  interface CSSProperties {
    '--tw-scale-x'?: string;
    '--tw-scale-y'?: string;
  }
}

// const globalStyles = `
// @keyframes fadeInLeft {
//   from {
//     opacity: 0;
//     transform: translateX(-20px);
//   }
//   to {
//     opacity: 1;
//     transform: translateX(0);
//   }
// }

// @keyframes fadeInRight {
//   from {
//     opacity: 0;
//     transform: translateX(20px);
//   }
//   to {
//     opacity: 1;
//     transform: translateX(0);
//   }
// }

// @keyframes slideUp {
//   from {
//     opacity: 0;
//     transform: translateY(20px);
//   }
//   to {
//     opacity: 1;
//     transform: translateY(0);
//   }
// }

// .animate-fade-in-left {
//   animation: fadeInLeft 1s ease-out;
// }

// .animate-fade-in-right {
//   animation: fadeInRight 1s ease-out;
// }

// .animate-slide-up {
//   animation: slideUp 0.8s ease-out;
// }

// .animate-fade-in {
//   animation: fadeIn 1s ease-out;
// }

// /* Add responsive breakpoint styles */
// @media (max-width: 640px) {
//   .animate-fade-in-left,
//   .animate-fade-in-right {
//     animation-duration: 0.8s;
//   }
// }
// `;

export default HomePage;