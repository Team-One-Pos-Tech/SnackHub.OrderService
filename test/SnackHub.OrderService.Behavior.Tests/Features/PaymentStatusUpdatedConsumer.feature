Feature: Payment Status Update Event Consumer test

    Scenario: Receive an update payment status 'approved'
        Given a valid order with product details:
          | ProductId                            | ProductName | UnitPrice | Quantity |
          | 919806ea-f94d-401b-a1f6-9a15291c4122 | X-Tudo      | 10.5      | 3        |
          | fea2f300-f37a-438f-9f10-f514ecd030b2 | Coca-cola   | 9.45      | 1        |
        When receiving an approved update event for that order
        Then the order should have status 'Confirmed'

    Scenario: Receive an update payment status 'rejected'
        Given a valid order with product details:
          | ProductId                            | ProductName | UnitPrice | Quantity |
          | 919806ea-f94d-401b-a1f6-9a15291c4122 | X-Tudo      | 10.5      | 3        |
          | fea2f300-f37a-438f-9f10-f514ecd030b2 | Coca-cola   | 9.45      | 1        |
        When receiving a reject update event for that order
        Then the order should have status 'Declined'