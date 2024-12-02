Feature: Confirm Order Use Case

    Scenario: Valid order, confirm should succeed
        Given a valid client with id '919806ea-f94d-401b-a1f6-9a15291c4125' and name 'John Doe'
        And a table of products with
          | Id                                   | Name      | Price | Description          |
          | 919806ea-f94d-401b-a1f6-9a15291c4122 | X-Tudo    | 10.5  | Completo sem abacaxi |
          | fea2f300-f37a-438f-9f10-f514ecd030b2 | Coca-cola | 9.45  | Coca gelada          |
        When confirming the order with client id '919806ea-f94d-401b-a1f6-9a15291c4125' and product details table:
          | ProductId                            | Quantity |
          | 919806ea-f94d-401b-a1f6-9a15291c4122 | 2        |
          | fea2f300-f37a-438f-9f10-f514ecd030b2 | 1        |
        Then should have '1' order
        And the order status should be 'Confirmed'

    Scenario: Invalid order, no products
        Given a valid client with id '919806ea-f94d-401b-a1f6-9a15291c4125' and name 'John Doe'
        Then confirming the order with client id '919806ea-f94d-401b-a1f6-9a15291c4125' and no product details, it should fail with message 'Order must have at least one item to be confirmed'

    Scenario: Invalid order, invalid client
        Given a valid client with id '919806ea-f94d-401b-a1f6-9a15291c4125' and name 'John Doe'
        Then confirming the order with client id '00000000-0000-0000-0000-000000000000' and no product details, it should fail with message 'Client not found'