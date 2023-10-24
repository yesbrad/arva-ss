import './index.css';
import { useState } from 'react';

const ManualUpdateModal = ({orderLine, onUpdate, onClose}) => {
	const [overrideNumber, SetOverrideNumber] = useState(0);

	console.log(orderLine);

	const productDescription = orderLine && orderLine.Product ? orderLine.Product.ProductDescription : '';
    const productCode = orderLine && orderLine.Product ? orderLine.Product.ProductCode : '';
    const orderQuantity = orderLine ? orderLine.OrderQuantity : '';

	return (
		<div className={`modal manual-modal ${orderLine && orderLine.Product ? "modal-on" : "modal-off"}`}>
			<h1>Manual Override</h1>
			<h2>{productDescription }</h2>
			<h5>{productCode }</h5>
			<h3>Order Quantity: {orderQuantity }</h3>
			<form onSubmit={(e) => {e.preventDefault(); onUpdate(overrideNumber,orderLine.Product.ProductCode); onClose();}}>
				<input onChange={e => SetOverrideNumber(e.currentTarget.value)} value={overrideNumber} type='number'></input>
				<button>Submit</button>
				<button type="button" onClick={onClose}>Close</button>
			</form>
			{/* <p>{ error.message.replace("(00000000-0000-0000-0000-000000000000)", "") }</p> */}
		</div>
	)
}

export default ManualUpdateModal;

