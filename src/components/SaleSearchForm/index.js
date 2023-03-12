import { useState, useRef, useEffect } from 'react';
import './index.css';

const SaleSearchForm = ({ onSearch, loaded }) => {
	const [orderQuery, setOrderQuery] = useState("SO-00015614");

	const onSearchForm = () => {
		onSearch(orderQuery);
	}

	return (
		<div className={`saleid-container ${loaded ? "sale-loaded" : ""}`}>
			<div className='saleid-wrapper'>
				<h2>Enter Order Number</h2>
				<form onSubmit={e => { e.preventDefault(); onSearchForm()}}>
					<input type="text" onChange={e => { e.preventDefault(); setOrderQuery(e.currentTarget.value) }} value={orderQuery} />
					<button>Search</button>
				</form>
			</div>
		</div>
	)
}

export default SaleSearchForm;