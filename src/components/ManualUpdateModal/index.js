import { API } from '../../data/auth';
import './index.css';
import { useState, useEffect } from 'react';

const ManualUpdateModal = ({orderLine, onUpdate, onClose}) => {
	const [overrideNumber, SetOverrideNumber] = useState(0);
	const [product, SetProduct] = useState({});

	useEffect(() => {
		LoadProduct();
	}, [orderLine])

	const LoadProduct = async () => {
		if(orderLine && orderLine.Product){
			console.log("l:oad: ", orderLine);
			let response = await API("GET", `Products?productCode=${orderLine.Product.ProductCode}`);
			
			SetProduct(response.Items[0]);
		}
	}

	const productDescription = orderLine && orderLine.Product ? orderLine.Product.ProductDescription : '';
    const productCode = orderLine && orderLine.Product ? orderLine.Product.ProductCode : '';
    const Barcode = product && product.Barcode ? product.Barcode : '';
    const BarcodeDec = product && product.Barcode ? product.Barcode : 'Missing!';
    const orderQuantity = orderLine ? orderLine.OrderQuantity : '';
    const pImg = product && product.ImageUrl ? product.ImageUrl : require('../../images/prev.png');

	return (
		<div className={`modal manual-modal ${orderLine && orderLine.Product ? "modal-on" : "modal-off"}`}>
			<h2>Manual Override</h2>
			<img src={pImg}></img>
			<h3>{productDescription}</h3>
			<h5>Unleashed Code: {productCode }</h5>
			<h5>Barcode: {BarcodeDec}</h5>
			<h4>Order Quantity: {orderQuantity }</h4>
			<form onSubmit={(e) => {e.preventDefault(); onUpdate(overrideNumber, Barcode); onClose();}}>
				<input onChange={e => SetOverrideNumber(e.currentTarget.value)} value={overrideNumber} type='number'></input>
				<button>Submit</button>
				<button type="button" onClick={onClose}>Close</button>
			</form>
			{/* <p>{ error.message.replace("(00000000-0000-0000-0000-000000000000)", "") }</p> */}
		</div>
	)
}

export default ManualUpdateModal;

