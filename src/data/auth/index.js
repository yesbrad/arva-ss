import CryptoJS from 'crypto-js';

let privateKey = process.env.REACT_APP_KEY;
let authID = process.env.REACT_APP_ID;

let testShipment = "4c58faa0-c9a2-4973-9750-8c8178b903f1";
let testOrder = "e5fa0336-c2eb-4a32-9940-235f99989858";

export const LoadSSFromOrderID = async (shipID) => {
	let res = await API("GET", `SalesShipments/?orderNumber=${shipID}`);

	if (res.Items.length !== 0) {
		return res.Items[0];
	} else {
		console.log("No Matching Shipment for Order ID")
		return  {Description: "No Matching Shipment for Order ID"};
	}
}

export const LoadSO = async (orderGuid) => {
	let data = await API("GET", `SalesOrders/${orderGuid}`);
	return data;
}

export const API = async (postType, endpoint, json) => {
	let sp = "";

	if (endpoint.split("?").length > 1) {
		var sar = endpoint.split("?");		
		sp = endpoint.replace(sar[0], "");
		sp = sp.replace("?", "");
	}

	var signature = GetAuthSignature(sp, privateKey);
	
	if(postType == "GET" ){
		let response = await fetch(`https://api.unleashedsoftware.com/${endpoint}`, {
			method: postType,
			headers: {
				"Content-Type": "application/json",
				"Accept": "application/json",
				"api-auth-id": authID,
				"api-auth-signature": signature,
			},
			
		}).catch(err => {
			console.log("API GET ERROR :: ", err)
			return err;
		})
		
		return await response.json();
	}
	else{
		let response = await fetch(`https://api.unleashedsoftware.com/${endpoint}`, {
			method: postType,
			headers: {
				"Content-Type": "application/json",
				"Accept": "application/json",
				"api-auth-id": authID,
				"api-auth-signature": signature,
			},
			body:  JSON.stringify(json),
			
		}).catch(err => {
			console.log("API ERROR :: ", err)
			return err;
		})

		return await response.json();
	}
	
}

const GetAuthSignature = (req, key) => {
	var hmac64 = CryptoJS.HmacSHA256(req, key);
	var hash64 = CryptoJS.enc.Base64.stringify(hmac64);
	return hash64;
}