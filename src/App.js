import './App.css';
import { useEffect, useState } from 'react';
import Heading from './components/Heading';
import Header from './components/Header';
import OrderLine from './components/OrderListing';
import { API, LoadSO, LoadSSFromOrderID } from './data/auth';
import StatusChecker from './components/StatusChecker';
import AddSKU from './components/AddSKU';
import ErrorModal from './components/ErrorModal';
import SaleSearchForm from './components/SaleSearchForm';
import Exit from './components/Exit';

const App = () => {
	const [ss, setSS] = useState({});
	const [so, setSO] = useState({});
	const [error, setError] = useState({
		title: "",
		message: "",
	});
	const [loaded, setLoaded] = useState(false);

	let testOrderNumber = "SO-00015614";

	useEffect(() => {
		//Load();
	}, [])

	const Load = async (orderNumber) => {

		let SS = await LoadSSFromOrderID(orderNumber);

		if (SS.Guid){
			setSS(SS);

			let SO = await LoadSO(SS.OrderGuid)
			setSO(SO);
			console.log(SS)

			setLoaded(true);
		}
		else {
			console.log(SS)
			SetError("Not FOUND", "No Matching Shipment for Order ID: " + orderNumber);
		}
	}

	const Close = () => {
		setSO({});
		setSS({});
		setLoaded(false);
	}

	const onAddSKU = (uCode, amount) => {
		let isCodePresent = false;

		console.log("UCODE::", uCode)

		for (let i = 0; i < ss.SalesShipmentLines.length; i++){
			if (ss.SalesShipmentLines[i].Product.ProductCode == uCode) isCodePresent = true;
		}

		let arr;

		if (isCodePresent) {
			//Increment
			arr = ss.SalesShipmentLines.map((line, i) => {
				if (line.Product.ProductCode == uCode) {
					return {...line, ShipmentQty: line.ShipmentQty + amount }
				}


				return line;
			})

			console.log("CODE IS PRESENT:: ", arr)

		} else {
			arr = ss.SalesShipmentLines.map(line => line);
			arr = [...arr, {
				Product: {
					ProductCode: uCode,
				},
				SerialNumbers: null,
				BatchNumbers: null,
				ShipmentQty: amount,
			}]

			console.log("ADD CODE::", arr)
		}

		UploadSS({
			...ss,
			SalesShipmentLines: arr,
		});
	}

	const UploadSS = async (_ss) => {
		// Updating SalesShipmentLines in dataBase
		let response = await API("PUT", `SalesShipments/${ss.Guid}`, {
			guid: _ss.Guid,
			orderNumber: _ss.OrderNumber ,
			shipmentStatus: _ss.ShipmentStatus ,
			SalesShipmentLines: _ss.SalesShipmentLines.map(line => {
				return {
					Product: line.Product,
					SerialNumbers: line.SerialNumbers,
					BatchNumbers: line.BatchNumbers,
					ShipmentQty: line.ShipmentQty,
					Guid: line.Guid,
					SalesOrderLineNumber: line.SalesOrderLineNumber,
				}
			}),
		})

		if (response.SalesShipmentLines) {
			
			console.log("SET SS!!!!")
			setSS({
				...ss,
				SalesShipmentLines: response.SalesShipmentLines,
			})
		}
		else {
			//console.log(response)
			//alert(response.Description)
			SetError("FAILURE", response.Description)
		}

		console.log("UPLOAD:: ", response);
	}

	const SetError = (title, message) => {
		setError({
			title,
			message,
		})

		setTimeout(() => {
			setError({
				title: "",
				message: "",
			})
		}, 2000)
	}

	return (
		<div className="container">
			<ErrorModal error={error} />
			<Exit onExit={Close} />
			<div className='content'>
				<SaleSearchForm onSearch={Load} loaded={loaded} />
				<Header>{ss && ss.ShipmentNumber}</Header>
				<Heading>Details</Heading>
				{so.Customer && <p>Name: {so.Customer.CustomerName}</p>}
				<p>Order Number: {so.OrderNumber}</p>
				<p>Address: {so.DeliveryStreetAddress}, {so.DeliverySuburb}, { so.DeliveryRegion}</p>
				<p>Status: {so.OrderStatus}</p>
				<Heading>Status</Heading>
				{(Object.keys(ss).length !== 0 && Object.keys(so).length !== 0)&& <StatusChecker so={so} ss={ss}></StatusChecker>}
				<Heading>Order/Shipment Lines</Heading>
				{so.SalesOrderLines !== undefined && so.SalesOrderLines.map(a => <OrderLine key={a.LineNumber} orderLine={a} shipment={ss}></OrderLine>)}
				<Heading>Scan SKU</Heading>
				<AddSKU onAddSKU={onAddSKU}/>
			</div>
		</div>
	);
}

export default App;
