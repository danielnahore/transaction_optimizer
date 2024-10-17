import React, { useState } from 'react';
import axios from 'axios';

function App() {
  const [formData, setFormData] = useState({
    apiLoginId: '',
    transactionKey: '',
    amount: '',
    cardNumber: '',
    expirationDate: '',
    cardCode: ''
  });

  const [message, setMessage] = useState('');

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post('http://localhost:5000/api/payment/process', formData);
      setMessage(response.data.message);
    } catch (error) {
      setMessage(error.response?.data || 'Payment Failed');
    }
  };

  return (
    <div>
      <h1>Authorize.Net Payment</h1>
      <form onSubmit={handleSubmit}>
        <input name="apiLoginId" placeholder="API Login ID" onChange={handleChange} required />
        <input name="transactionKey" placeholder="Transaction Key" onChange={handleChange} required />
        <input name="amount" type="number" placeholder="Amount" onChange={handleChange} required />
        <input name="cardNumber" placeholder="Card Number" onChange={handleChange} required />
        <input name="expirationDate" placeholder="Expiration Date (MMYY)" onChange={handleChange} required />
        <input name="cardCode" placeholder="Card Code (CVV)" onChange={handleChange} required />
        <button type="submit">Pay</button>
      </form>
      {message && <p>{message}</p>}
    </div>
  );
}

export default App;
