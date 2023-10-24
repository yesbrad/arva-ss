import Heading from "../Heading";
import './index.css';

const OrderLine = (props) => {
	const GetShipmentLine = () => {
		let Num = 0;

		props.shipment.SalesShipmentLines.map(line => {
			if (line.SalesOrderLineNumber == props.orderLine.LineNumber) {
				Num = line.ShipmentQty;
			}
		})

		return Num;
	}

	const getColor = () => {
		if (GetShipmentLine() == props.orderLine.OrderQuantity) return "rgb(21, 202, 111)";
		if (GetShipmentLine() == 0) return "lightGray"
		if (GetShipmentLine() < props.orderLine.OrderQuantity) return "orange";
		return "gray";
	}

	return (
		<div class="order-container" onClick={() => props.onSelect()}>
			<div className="order-name">
				<span>
					<p>{props.orderLine.Product.ProductDescription}</p>
					<p class="order-code">{props.orderLine.Product.ProductCode}</p>
				</span>
			</div>
			<div className="order-qty-color" style={{backgroundColor: getColor()}}>
				<h4 className="order-qty">{GetShipmentLine()}/{props.orderLine.OrderQuantity}</h4>
			</div>
		</div>
	);
}

export default OrderLine;