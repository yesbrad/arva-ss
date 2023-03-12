import { useEffect, useState } from 'react';
import './index.css';

const StatusChecker = ({ss, so}) => {
	const [good, setGood] = useState(false);

	useEffect(() => {
		let isGood = true;

		so.SalesOrderLines.map(line => {
			let inShipment = false;

			ss.SalesShipmentLines.map(sLine => {
				if (line.LineNumber == sLine.SalesOrderLineNumber) {
					inShipment = true;
					if (line.OrderQuantity != sLine.ShipmentQty) {
						isGood = false;
					}
				}
			})
			
			if (inShipment == false)
				isGood = false;
		
		})

		setGood(isGood);
		//};
	});

	return (
		<div className={`status-container ${good ? "good" : ""}`}>
			<span>{ good ? "All Good" : "Missing Products"}</span>
		</div>
	)
}

export default StatusChecker;