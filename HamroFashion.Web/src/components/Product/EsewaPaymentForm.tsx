import React, { useState } from "react";

const EsewaPaymentForm: React.FC = () => {
  const [formData, setFormData] = useState({
    totalAmount: "110",
    transactionUuid: "241028",
    productCode: "EPAYTEST",
  });

  const [signature, setSignature] = useState<string>("");

  const generateSignature = async () => { 
    try { 
      const response = await fetch("/api/Esewa/GenerateSignature", 
        { 
          method: "POST", 
          headers: { "Content-Type": "application/json", }, 
          body: JSON.stringify(formData),
        }); 
        const data = await response.json(); 
        window.location.href = data.paymentUrl;

        setSignature(data.signature); } 
    catch (error) {
        console.error("Error generating signature", error); 
    } 
    };
  return (
    <div>
      <h1>eSewa Payment</h1>
      <form
        action="https://rc-epay.esewa.com.np/api/epay/main/v2/form"
        method="POST"
      >
        <input type="hidden" name="total_amount" value={formData.totalAmount} />
        <input
          type="hidden"
          name="transaction_uuid"
          value={formData.transactionUuid}
        />
        <input type="hidden" name="product_code" value={formData.productCode} />
        <input type="hidden" name="success_url" value="https://your-merchant-app.com/success" />
        <input type="hidden" name="failure_url" value="https://your-merchant-app.com/failure" />
        <input type="hidden" name="signed_field_names" value="total_amount,transaction_uuid,product_code" />
        <input type="hidden" name="signature" value={signature} />
        <button type="submit" onClick={generateSignature}>
          Pay with eSewa
        </button>
      </form>
    </div>
  );
};

export default EsewaPaymentForm;
