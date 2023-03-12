import { useState } from "react";
import QtySelector from "../QtySelector";
import "./index.css";

const AddSKU = ({ onAddSKU }) => {
	//8127
	const [SKU, setSKU] = useState("8127");
	const [addAmount, setAddAmount] = useState(1);

	const onInput = (e) => {
		e.preventDefault();
		setSKU(e.currentTarget.value)
	}
	
	return (
		<div className="add-container">
			<input
				onChange={onInput}
				value={SKU}
			></input>
			<QtySelector value={addAmount} onChange={setAddAmount} />
			<button onClick={() => { console.log(SKU); onAddSKU(SKU, addAmount); }}>Add SKU</button>
		</div>
	)
}

export default AddSKU;