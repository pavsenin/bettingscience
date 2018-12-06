import React, {PureComponent} from 'react'

class Article extends PureComponent {
    constructor(props) {
        super(props)
        this.state = {
            isOpen : props.defaultOpen,
            count : 0
        }
    }
    componentWillReceiveProps(nextProps) {
        if(nextProps.defaultOpen !== this.props.isOpen)
            this.setState({
                isOpen : nextProps.defaultOpen
            })
    }
    componentWillUpdate() {
        console.log('---', 'will update')
    }
    render() {
        const {article} = this.props
        const body = this.state.isOpen && <section className="card-text">{article.text}</section>
        return (
            <div className="card mx-auto" style={{width:'50%'}}>
                <div className="card-header">
                    <h2 onClick={this.incrementCount}>
                        {article.title}
                        counted {this.state.count}
                        <button className="btn btn-primary btn-lg float-right" onClick={this.handleClick}>
                            {this.state.isOpen ? 'close':'open'}
                        </button>
                    </h2>
                </div>
                <div className="card-body">
                    <h6 className="card-subtitle text-muted">
                        Creation date: {new Date(article.date).toDateString()}
                    </h6>
                    {body}
                </div>
            </div>
        )
    }
    incrementCount = () => {
        this.setState({
            count : this.state.count + 1
        })
    }
    handleClick = () => {
        this.setState({
            isOpen : !this.state.isOpen
        })
    }
}

export default Article